import {ChatMessage} from "./chatMessage";
import {ChatUser} from "./chatUser";

export class Chat {
  public id: string = "";
  public name: string | null = "";
  public lastmessage: string | null = "";
  public messages: ChatMessage[] = [];
  public chatUsers: ChatUser[] = [];
}
