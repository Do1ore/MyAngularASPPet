import { AppUser } from "./appUser";
import { ChatMainModel } from "./chatMainModel";

export class ChatMessage {
  public id: string = '';
  public content: string = '';
  public sentAt: string = '';
  public senderId: string = '';
  public sender: AppUser | null = null;
  public chatId: string = '';
  public chat: ChatMainModel | null = null;
}
