import { ChatMessage } from "./chatMessage";


import { UserProfileImage } from "./userProfileImage";

export interface AppUser {
  id: string;
  username: string;
  surname: string;
  email: string;
  passwordHash: string;
  passwordSalt: string;
  refreshToken: string;
  tokenCreated: string;
  tokenExpires: string;
  accountCreated: string;
  accountLastTimeEdited: string;
  lastTimeOnline: string;
  userProfileImages: UserProfileImage[];
  sentMessages: ChatMessage[];
  receivedMessages: ChatMessage[];
}
