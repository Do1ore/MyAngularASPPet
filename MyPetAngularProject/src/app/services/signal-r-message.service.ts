import {Injectable} from '@angular/core';
import {HubConnection, HubConnectionBuilder} from '@microsoft/signalr';
import {environment} from "../../environments/environment";
import {Subject} from "rxjs";
import {ChatMainModel} from '../models/chatMainModel';
import {JwtHelperService} from "@auth0/angular-jwt";
import {ToastrService} from "ngx-toastr";
import {ChatMessage} from "../models/chatMessage";
import {Message} from "postcss";

@Injectable({
  providedIn: 'root'
})
export class SignalRMessageService {

  private hubConnection: HubConnection | null = null;
  private baseApiUrl = environment.baseApiUrl;
  private authTokenName = environment.authTokenName;
  public modelSubject: Subject<ChatMainModel[]> = new Subject<ChatMainModel[]>();
  public model$ = this.modelSubject.asObservable();

  public chatModel: ChatMainModel = new ChatMainModel();
  public chatDetailsSubject: Subject<ChatMainModel> = new Subject<ChatMainModel>();
  public chatDetails$ = this.modelSubject.asObservable();

  private userId: string = '';

  constructor(private jwtHelper: JwtHelperService, private toaster: ToastrService) {
    this.userId = this.getUserIdFromToken();
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
    if (!this.hubConnection) {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(this.baseApiUrl + 'hub/user')
        .build();

      this.hubConnection.start()
        .then(() => {
          this.getAllChatsForUserListener();
        })
        .catch(err => console.error('Error while starting SignalR connection:', err));
    }
    return this.hubConnection;
  }

  public getAllChatsForUserCaller() {
    if (this.hubConnection === null || this.hubConnection.state != 'Connected') {
      console.log("not yet1")
      return;
    }

    console.log("yes1");
    this.hubConnection!.invoke('GetAllChatsForUser', this.userId)
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
    this.hubConnection!.invoke('GetChatsDetails', this.userId, chatId)
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
    if (this.userId.length === 0) {
      return;
    }
    return this.hubConnection.invoke('SendMessage', chatId, this.userId, message);
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


}
