import { ChatMessage } from "./chatMessage";
import { ChatUser } from "./chatUser";

export interface Chat {
  id: string;
  name: string | null;
  lastmessage: string | null;
  messages: ChatMessage[];
  chatUsers: ChatUser[];
}
