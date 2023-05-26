import { AppUser } from "./appUser";
import { Chat } from "./chat";

export class ChatMessage {
  public id: string = '';
  public content: string | null = null;
  public sentAt: string = '';
  public senderId: string = '';
  public sender: AppUser | null = null;
  public chatId: string = '';
  public chat: Chat | null = null;
}
