import {AppUser} from "./appUser";
import {ChatMainModel} from "./chatMainModel";

export class ChatUser {
  public userId: string = "";
  public user: AppUser | null = null;
  public chatId: string = "";
  public chat: ChatMainModel | null = null;
}
