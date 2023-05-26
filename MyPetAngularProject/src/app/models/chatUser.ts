import { AppUser } from "./appUser";
import { Chat } from "./chat";

export interface ChatUser {
  userId: string;
  user: AppUser | null;
  chatId: string;
  chat: Chat | null;
}
