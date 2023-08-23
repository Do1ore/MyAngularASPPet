import {Component, Input, OnInit} from '@angular/core';
import {Modal} from "flowbite";
import {ModalHelperService} from "../../services/Ui/modal-helper.service";
import {HttpClient, HttpEventType} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {ChatService} from "../../services/chat.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-edit-chat-modal',
  templateUrl: './edit-chat-modal.component.html',
  styleUrls: ['./edit-chat-modal.component.scss']
})

export class EditChatModalComponent implements OnInit {

  @Input() modalId: string = '';
  @Input() chatId: string = ''

  constructor(
    private modalHelper: ModalHelperService,
    private http: HttpClient,
    private chatService: ChatService,
    private toaster: ToastrService,
  ) {
  }

  private editModal: Modal = new Modal();
  private baseApiUrl = environment.baseApiUrl + 'api';
  fileName: string = "";

  initializeModal() {
    return this.modalHelper.initializeModal(this.modalId);
  }

  ngOnInit(): void {
    this.editModal = this.initializeModal()
  }

  close() {
    this.editModal.hide()
  }

  onFileSelected(event: any) {

    const file: File = event.target.files[0];

    if (file) {

      this.fileName = file.name;

      const formData = new FormData();

      let currentChatId = this.chatService.currentChatId;
      if (!currentChatId) {
        this.toaster.error("Current chat id is null", "Internal error")
      }
      formData.append("image", file);
      formData.append("chatId", currentChatId!);
      const upload$ = this.http.post(this.baseApiUrl + '/Image/upload-chat-image', formData, {}).subscribe(() => {
      }, (err) => {
        console.error(err);
      });

    }
  }


}
