import {ChatMessage} from "./chatMessage";
import {ChatUser} from "./chatUser";

export class ChatMainModel {
  public id: string = "";
  public name: string = "";
  public lastmessage: string = "";
  public messages: ChatMessage[] = [];
  public chatUsers: ChatUser[] = [];
}
