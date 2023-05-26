import {AppUser} from "./appUser";
import {Chat} from "./chat";

export class ChatUser {
  public userId: string = "";
  public user: AppUser | null = null;
  public chatId: string = "";
  public chat: Chat | null = null;
}
