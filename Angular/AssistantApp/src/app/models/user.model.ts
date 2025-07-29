export interface User {
  id: number;
  name: 'user' | 'bot';
}

export const USER_BOT: User = { id: 1, name: 'bot' };
export const USER_USER: User = { id: 2, name: 'user' };

export const USERS: User[] = [
  USER_BOT,
  USER_USER
];