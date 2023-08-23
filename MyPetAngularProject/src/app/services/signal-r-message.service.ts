import {Injectable} from '@angular/core';
import {HubConnection, HubConnectionBuilder} from '@microsoft/signalr';
import {environment} from "../../environments/environment";
import {Subject} from "rxjs";
import {ChatMainModel} from '../models/chatMainModel';
import {JwtHelperService} from "@auth0/angular-jwt";
import {ToastrService} from "ngx-toastr";
import {ChatMessage} from "../models/chatMessage";
import {CreateChatDto} from "../models/createChatDto";

@Injectable({
  providedIn: 'root'
})
export class SignalRMessageService {

  public modelSubject: Subject<ChatMainModel[]> = new Subject<ChatMainModel[]>();
  public model$ = this.modelSubject.asObservable();
  public chatModel: ChatMainModel = new ChatMainModel();
  public chatDetailsSubject: Subject<ChatMainModel> = new Subject<ChatMainModel>();
  public chatDetails$ = this.modelSubject.asObservable();
  private hubConnection: HubConnection | null = null;
  private baseApiUrl = environment.baseApiUrl;
  private authTokenName = environment.authTokenName;


  private signalRConnectedSubject: Subject<void> = new Subject<void>();
  public signalRConnect$ = this.signalRConnectedSubject.asObservable();


  constructor(private jwtHelper: JwtHelperService, private toaster: ToastrService) {
  }

  public getUserIdFromToken(): string {
    const token = localStorage.getItem(this.authTokenName);
    if (token === null) {
      return '';
    }
    const decodedToken = this.jwtHelper.decodeToken(token);
    return decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
  }

  public getHubConnection(): HubConnection {
    console.log('method started!')
    if (!this.hubConnection || this.hubConnection?.state !== 'Connected') {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(this.baseApiUrl + 'hub/user')
        .withAutomaticReconnect()
        .build();

      this.hubConnection.start()
        .then(() => {
          this.signalRConnectedSubject.next();
          this.getAllChatsForUserListener();
        })
        .catch(err => console.error('Error while starting SignalR connection:', err));
    }
    return this.hubConnection;
  }

  public disconnectFromHub() {
    if (this.hubConnection) {
      this.hubConnection.stop().then();
      console.log('Connection with hub closed');
    }
  }

  public getAllChatsForUserCaller() {
    if (this.hubConnection === null || this.hubConnection.state != 'Connected') {
      console.log("not yet1")
      return;
    }
    let userId = this.getUserIdFromToken();

    console.log("yes1");
    this.hubConnection!.invoke('GetAllChatsForUser', userId)
      .catch(err => console.error(err));
  }

  public getAllChatsForUserListener() {
    if (this.hubConnection === null || this.hubConnection.state != 'Connected') {
      console.log("not yet1")
      return;
    }
    console.log("yes2")

    this.hubConnection.on('GetAllChatsForUserResponse', (model: ChatMainModel[]) => {
      console.info(model);
      model.forEach((model) => {
        console.log(model.id);
      });
      this.modelSubject.next(model);
    });
  }

  //CHAT DETAILS//
  public getChatDetailsCaller(chatId: string) {
    if (this.hubConnection === null || this.hubConnection.state != 'Connected') {
      return;
    }
    let userId = this.getUserIdFromToken();

    this.hubConnection!.invoke('GetChatsDetails', userId, chatId)
      .catch(err => console.error(err));
  }

  public getChatDetailsListener() {
    if (this.hubConnection === null || this.hubConnection.state != 'Connected') {
      return;
    }
    this.hubConnection.on('GetChatsDetailsResponse', (model: ChatMainModel) => {
      console.info(model);
      this.chatDetailsSubject.next(model);
    });
  }

  public sendMessageCaller(message: string, chatId: string) {
    if (this.hubConnection === null || this.hubConnection.state != 'Connected') {
      return;
    }
    this.hubConnection.on('GetChatsDetailsResponse', (model: ChatMainModel) => {
      this.chatDetailsSubject.next(model);
    });
  }

  //MESSAGES//
  joinChat(chatId: string) {
    if (this.hubConnection === null || this.hubConnection.state != 'Connected') {
      return;
    }
    return this.hubConnection.invoke('JoinChat', chatId);
  }

  leaveChat(chatId: string) {
    if (this.hubConnection === null || this.hubConnection.state != 'Connected') {
      return;
    }

    return this.hubConnection.invoke('LeaveChat', chatId);
  }


  public sendMessage(chatId: string, message: string) {
    if (this.hubConnection === null || this.hubConnection.state != 'Connected') {
      return;
    }
    let userId = this.getUserIdFromToken();

    if (userId.length === 0) {
      return;
    }
    return this.hubConnection.invoke('SendMessage', chatId, userId, message);
  }

  public onReceiveMessage(callback: (message: ChatMessage) => void): void {
    if (this.hubConnection === null || this.hubConnection.state != 'Connected') {
      return;
    }
    this.hubConnection.on('ReceiveMessage', (message: ChatMessage) => {
      callback(message);
    });
  }

  public onReceiveLastMessage(callback: (chatId: string, message: string) => void): void {
    if (this.hubConnection === null || this.hubConnection.state != 'Connected') {
      return;
    }
    this.hubConnection.on('ReceiveLastMessage', (chatId: string, message: string) => {
      callback(chatId, message);
    });

  }

  //CHATS//
  public createChatCaller(chatDto: CreateChatDto) {
    if (this.hubConnection === null || this.hubConnection.state != 'Connected') {
      return;
    }
    return this.hubConnection.invoke('CreateChat', chatDto);
  }

  public createChatListener(callback: (chat: ChatMainModel) => void): void {
    if (this.hubConnection === null || this.hubConnection.state != 'Connected') {
      return;
    }
    this.hubConnection.on('CreateChatResponse', (chat: ChatMainModel) => {
      callback(chat);
    });

  }

  public deleteChatCaller(chatId: string) {
    if (this.hubConnection === null || this.hubConnection.state != 'Connected') {
      return;
    }
    this.hubConnection?.invoke('DeleteChat', chatId);
  }

  public deleteChatListener(callback: (chatId: string) => void): void {
    console.log('chat deleted');
    this.hubConnection?.on('DeleteChatResponse', callback);
  }

}
