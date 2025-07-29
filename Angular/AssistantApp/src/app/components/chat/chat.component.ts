import { Component, OnInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatFormFieldModule } from '@angular/material/form-field';
import { Client, MessageDto, RateMessageDto } from '../../services/api.services';
import { USER_BOT, USER_USER, USERS } from '../../models/user.model';


interface Message {
  id: number;
  date?: Date,
  text: string;
  sender: 'user' | 'bot';
  senderId: number;
  rating?: 'like' | 'dislike' | null;
  isGenerating?: boolean;
  fullText?: string;
  isReaded?: boolean;
}

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
  standalone: true,
  imports: [    
    CommonModule ,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatListModule,
    MatFormFieldModule],
    providers: [Client]
})
export class ChatComponent implements OnInit, OnDestroy {
  messages: Message[] = [];
  userInput = new FormControl('');
  isGenerating = false;
  currentMessageId: number | null = null;
  
  @ViewChild('scrollContainer') private scrollContainer!: ElementRef;

  constructor(
    private _apiService: Client
  ) {}

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.cancelGeneration();
  }

  getFormattedMsg(msg: string) {
    return msg.replace(/\n/g, '<br>');
  }

  sendMessage() {
    const userText = this.userInput?.value?.trim();
    if (!userText || this.isGenerating) return;

    const userMsg: Message = {
      id: 0,
      text: userText,
      sender: USER_USER.name,
      senderId: USER_USER.id,
      date: new Date()
    };
    this.messages.push(userMsg);
    this.userInput.reset();
    this.scrollToBottom();
    this.generateBotResponse();
  }

  generateBotResponse() {
    const botMsg: Message = {
      id: 0,
      text: '',
      sender: USER_BOT.name,
      senderId: USER_BOT.id,
      isGenerating: true,
      fullText: ''
    };
    const questionMsg = this.messages[this.messages.length - 1];
    const questionMsgDto = new MessageDto({
      id: questionMsg.id,
      userIdTo: USER_BOT.id,
      userIdFrom: USER_USER.id,
      date: questionMsg.date,
      isReaded: questionMsg.isReaded,
      text: questionMsg.text
    });

    this.messages.push(botMsg);
    this.isGenerating = true;
    this.currentMessageId = botMsg.id;

    this._apiService.askQuestion(questionMsgDto).subscribe({
      next: (response) => {

        if (!response.id){
          botMsg.isGenerating = false;
          this.isGenerating = false;
          this.currentMessageId = null;
          return;
        }

        this.currentMessageId = response.id;
        botMsg.id = response.id ?? 0;
        botMsg.text = ""
        botMsg.fullText = response.text?? "";
        botMsg.date = response.date;
        questionMsg.id = response.questionId ?? 0;
        this.scrollToBottom();        
        this.generateWordByWord(botMsg);
      },
      error: () => {
        botMsg.isGenerating = false;
        this.isGenerating = false;
      },
      complete: () => {
        this.scrollToBottom();
      }
    });
  }

  cancelGeneration() {
    const botMsg = this.messages.find(m => m.id === this.currentMessageId);
    if (botMsg && botMsg.isGenerating) {
      botMsg.isGenerating = false;
      botMsg.isReaded = true;
      this.isGenerating = false;
      this.saveResponse(botMsg);
    }
    this.currentMessageId = null;
  }

  rateResponse(messageId: number, rating: 'like' | 'dislike') {
    const msg = this.messages.find(m => m.id === messageId);
    if (msg) {
      msg.rating = rating;
      this._apiService
        .rateMessage(new RateMessageDto({messageId: messageId, rate: (rating === 'like' ? 1 : -1)}))
        .subscribe({
          next: (response) => {
            if (!response){
              console.log(`Failed to save rating for message ${messageId}: ${rating}`);
              alert("Wystąpił błąd zapisu oceny dla wiadomości.");
            } 
          },
          error: (error) => {
            alert(`Wystąpił błąd zapisu oceny dla wiadomości:   ${messageId}: ${rating} Błąd: ${error}`);
          }});
    }
  }

  getHistory() {
    const firstElementId: number = this.messages?.[0]?.id ?? -1;

    this._apiService.getHistory(firstElementId, 10).subscribe({
      next: (response) =>{ 
        const historyMessageList: Message[] = response.map(dto => this.fromMessageDto(dto));
        this.messages.unshift(...historyMessageList);
        this.scrollToTop();
      }
    });
  }

  private toMessageDto(model: Message): MessageDto {
    let dto = new MessageDto();
    dto.id = model.id;
    dto.date = model.date;
    dto.userIdFrom= model.senderId;
    dto.isReaded = model.isReaded;
    dto.text = model.text;
    dto.rate = (!model.rating) ? 0 : (model.rating === 'like' ? 1 : -1);
    return dto;
  }

  private fromMessageDto(dto: MessageDto): Message {
    const model: Message = {
      id: dto.id ?? 0,
      date: dto.date,
      senderId: dto.userIdFrom ?? 0,
      sender: USERS.find(x => x.id === dto.userIdFrom)?.name ?? 'bot',
      isReaded : dto.isReaded,
      text: dto.text ?? "",
      rating: dto.rate == 0 ? null : (dto.rate === 1 ? 'like' : 'dislike')
    };
    return model;
  }

  private generateWordByWord(message: Message){
    const words = (message.fullText ?? "").split(' ');
    const totalWords = words.length;

    let currentIndex: number = 0;
    let intervalId: any;

    intervalId = setInterval(() => {
      if (currentIndex < words.length && this.isGenerating && message.isGenerating && this.currentMessageId === message.id) {

        message.text += words[currentIndex] + ' ';
        currentIndex++;
        this.scrollToBottom();
      } else {

        message.isGenerating = false;
        this.isGenerating = false;
        this.currentMessageId = null;
        clearInterval(intervalId);

        message.isReaded = true;
        const dto = this.toMessageDto(message);
        this.scrollToBottom();

        this._apiService.saveAnswear(dto).subscribe();
      }
    }, 50); 
  }

  private saveResponse(message: { id: number; text: string; senderId: number, sender?: string; fullText?: string}) {
    const newMessageDto: MessageDto = new MessageDto({id:message.id, isReaded: true, text: message.fullText, userIdFrom: message.senderId});
      this._apiService.saveAnswear(newMessageDto).subscribe(response => {
    });
  }

  private scrollToBottom() {
    setTimeout(() => {
      if (this.scrollContainer && this.scrollContainer.nativeElement) {
        this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
      }
    }, 10);
  }

  private scrollToTop() {
    setTimeout(() => {
      if (this.scrollContainer && this.scrollContainer.nativeElement) {
        this.scrollContainer.nativeElement.scrollTop = 0;
      }
    }, 10);
  }
  
}