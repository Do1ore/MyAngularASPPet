export interface Chat {
  id: string;
  name: string | null;
  lastmessage: string | null;
  messages: ChatMessage[];
  chatUsers: ChatUser[];
}
