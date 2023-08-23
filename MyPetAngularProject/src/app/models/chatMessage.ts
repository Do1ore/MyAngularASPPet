import {AppUser} from "./appUser";

export class ChatMessage {
  public id: string = '';
  public content: string = '';
  public sentAt: string = '';
  public senderId: string = '';
  public chatId: string = '';
  //Uses only in client
  public senderInformation: AppUser = new AppUser();
}
