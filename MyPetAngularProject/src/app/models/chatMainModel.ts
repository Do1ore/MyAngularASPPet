import {ChatMessage} from "./chatMessage";
import {AppUser} from "./appUser";

export class ChatMainModel {
  public id: string = "";
  public name: string = "";
  public chatAdministrator: string = "";
  public lastMessage: string = "";
  public messages: ChatMessage[] = [];
  public appUsers: AppUser[] = [];
}
