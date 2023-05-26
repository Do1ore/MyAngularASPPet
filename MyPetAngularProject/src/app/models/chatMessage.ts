import { AppUser } from "./appUser";
import { Chat } from "./chat";

export interface ChatMessage {
  id: string;
  content: string | null;
  sentAt: string;
  senderId: string;
  sender: AppUser | null;
  chatId: string;
  chat: Chat | null;
}
