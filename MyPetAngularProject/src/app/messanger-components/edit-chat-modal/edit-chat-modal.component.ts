import {Component, Input, OnInit} from '@angular/core';
import {Modal} from "flowbite";
import {ModalHelperService} from "../../services/Ui/modal-helper.service";

@Component({
    selector: 'app-edit-chat-modal',
    templateUrl: './edit-chat-modal.component.html',
    styleUrls: ['./edit-chat-modal.component.scss']
})

export class EditChatModalComponent implements OnInit {

    @Input() modalId: string = '';

    constructor(private modalHelper: ModalHelperService) {
    }

    private editModal: Modal = new Modal();

    initializeModal() {
        return this.modalHelper.initializeModal(this.modalId);
    }

    ngOnInit(): void {
        this.editModal = this.initializeModal()
    }

    close(){
        this.editModal.hide()
    }


}
