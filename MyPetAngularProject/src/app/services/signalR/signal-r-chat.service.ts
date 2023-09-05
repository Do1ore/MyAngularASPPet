import {Injectable} from '@angular/core';
import {ChatMainModel} from "../../models/chatMainModel";
import {SignalRConnectionService} from "./signalr-connection.service";
import {LocalStorageHelperService} from "../local-storage-helper.service";
import {CreateChatDto} from "../../models/createChatDto";

@Injectable({
  providedIn: 'root'
})
export class SignalRChatService {

  public chatMainModel: ChatMainModel[] = [];
  public currentChatId: string = '';

  constructor(private signalRConnection: SignalRConnectionService, private readonly storageService: LocalStorageHelperService) {
  }

  //CHAT DETAILS//
  public getChatDetailsCaller(chatId: string) {
    if (this.signalRConnection.hubConnection === null || this.signalRConnection.hubConnection.state != 'Connected') {
      return;
    }
    let userId = this.storageService.getUserIdFromToken();

    this.signalRConnection.hubConnection!.invoke('GetChatsDetails', userId, chatId)
      .catch(err => console.error(err));
  }

  public getChatDetailsListener(callback: (chats: ChatMainModel) => void) {
    if (this.signalRConnection.hubConnection === null || this.signalRConnection.hubConnection.state != 'Connected') {
      return;
    }
    this.signalRConnection.hubConnection.on('GetChatsDetailsResponse', (model: ChatMainModel) => {
      console.info(model);
      callback(model);
    });
  }

  public deleteChatCaller(chatId: string) {
    if (this.signalRConnection.hubConnection === null || this.signalRConnection.hubConnection.state != 'Connected') {
      return;
    }
    this.signalRConnection.hubConnection?.invoke('DeleteChat', chatId);
  }

  public deleteChatListener(callback: (chatId: string) => void): void {
    this.signalRConnection.hubConnection?.on('DeleteChatResponse', callback);
  }


  //CHATS//
  public createChatCaller(chatDto: CreateChatDto) {
    if (this.signalRConnection.hubConnection === null || this.signalRConnection.hubConnection.state != 'Connected') {
      return;
    }
    console.log('Create chat invoking with chatDto: ', chatDto);
    this.signalRConnection.hubConnection.invoke('CreateChat', chatDto).catch((error) => {
      console.error('Ошибка при вызове метода SignalR', error);
    });
  }

  public createChatListener(callback: (chat: ChatMainModel) => void): void {
    if (this.signalRConnection.hubConnection === null || this.signalRConnection.hubConnection.state != 'Connected') {
      return;
    }
    this.signalRConnection.hubConnection.on('CreateChatResponse', (chat: ChatMainModel) => {
      callback(chat);
    });

  }

  public getAllChatsForUserCaller() {
    if (!this.signalRConnection.isConnected()) {
      console.log("No connection")
      return;
    }
    let userId = this.storageService.getUserIdFromToken();

    console.log("yes1");
    this.signalRConnection.hubConnection?.invoke('GetAllChatsForUser', userId)
      .catch(err => console.error(err));
  }

  public getAllChatsForUserListener(callback: (chats: ChatMainModel[]) => void) {
    if (this.signalRConnection.hubConnection === null || !this.signalRConnection.isConnected()) {
      console.log("No connection")
      return;
    }
    this.signalRConnection.hubConnection.on('GetAllChatsForUserResponse', (model: ChatMainModel[]) => {
      console.info(model);
      model.forEach((model) => {
        console.log(model.id);
      });
      callback(model);
    });
  }

  joinChat(chatId: string) {
    if (this.signalRConnection.hubConnection === null || this.signalRConnection.hubConnection.state != 'Connected') {
      return;
    }
    return this.signalRConnection.hubConnection.invoke('JoinChat', chatId);
  }

  leaveChat(chatId: string) {
    if (this.signalRConnection.hubConnection === null || this.signalRConnection.hubConnection.state != 'Connected') {
      return;
    }

    return this.signalRConnection.hubConnection.invoke('LeaveChat', chatId);
  }
}
