import {Injectable} from '@angular/core';
import {environment} from "../../../environments/environment";
import {Subject} from "rxjs";
import {ChatMainModel} from '../../models/chatMainModel';
import {JwtHelperService} from "@auth0/angular-jwt";
import {ToastrService} from "ngx-toastr";
import {ChatMessage} from "../../models/chatMessage";
import {CreateChatDto} from "../../models/createChatDto";
import {SignalRConnectionService} from "./signalr-connection.service";
import {LocalStorageHelperService} from "../local-storage-helper.service";


@Injectable({
    providedIn: 'root'
})
export class SignalRMessageService {

    public modelSubject: Subject<ChatMainModel[]> = new Subject<ChatMainModel[]>();
    public model$ = this.modelSubject.asObservable();
    public chatModel: ChatMainModel = new ChatMainModel();
    public chatDetailsSubject: Subject<ChatMainModel> = new Subject<ChatMainModel>();
    public chatDetails$ = this.modelSubject.asObservable();


    constructor(private signalRConnection: SignalRConnectionService, private storageService: LocalStorageHelperService) {
    }


    public sendMessageCaller(message: string, chatId: string) {
        if (this.signalRConnection.hubConnection === null || this.signalRConnection.hubConnection.state != 'Connected') {
            return;
        }
        this.signalRConnection.hubConnection.on('GetChatsDetailsResponse', (model: ChatMainModel) => {
            this.chatDetailsSubject.next(model);
        });
    }

    //MESSAGES//
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


    public sendMessage(chatId: string, message: string) {
        if (this.signalRConnection.hubConnection === null || this.signalRConnection.hubConnection.state != 'Connected') {
            return;
        }
        let userId = this.storageService.getUserIdFromToken();

        if (userId.length === 0) {
            return;
        }
        return this.signalRConnection.hubConnection.invoke('SendMessage', chatId, userId, message);
    }

    public onReceiveMessage(callback: (message: ChatMessage) => void): void {
        if (this.signalRConnection.hubConnection === null || this.signalRConnection.hubConnection.state != 'Connected') {
            return;
        }
        this.signalRConnection.hubConnection.on('ReceiveMessage', (message: ChatMessage) => {
            callback(message);
        });
    }

    public onReceiveLastMessage(callback: (chatId: string, message: string) => void): void {
        if (this.signalRConnection.hubConnection === null || this.signalRConnection.hubConnection.state != 'Connected') {
            return;
        }
        this.signalRConnection.hubConnection.on('ReceiveLastMessage', (chatId: string, message: string) => {
            callback(chatId, message);
        });

    }




}
