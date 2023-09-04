import {Component, OnInit} from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {Subscription} from "rxjs";
import {SignalRChatService} from "../../services/signalR/signal-r-chat.service";


@Component({
  selector: 'app-chat-domain',
  templateUrl: './chat-domain.component.html',
  styleUrls: ['./chat-domain.component.scss']
})
export class ChatDomainComponent implements OnInit {
  constructor(
    public authService: AuthService,
    private router: Router,
    public toaster: ToastrService,
    private chatService: SignalRChatService,
  ) {
  }

  private loginSubscription: Subscription = new Subscription();
  selectedChatId: string = '';

  public showChatDetails(chatId: string) {
    this.selectedChatId = chatId;
    this.chatService.currentChatId = this.selectedChatId;
    console.log('Current chat id: ', this.selectedChatId)
  }

  ngOnInit() {
    this.loginSubscription = this.authService.logout$.subscribe(() => {
      this.router.navigate(['/login']).then();
      this.toaster.warning('To use chat you need to be authorized', 'Account required👁️');
      return;
    });
    if (!this.authService.isAuthorized()) {
      this.router.navigate(['/login']).then();
      this.toaster.warning('To use chat you need to be authorized', 'Account required💀');
      return;
    }
  }

}
