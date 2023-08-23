import {Injectable} from '@angular/core';
import {Modal, ModalOptions} from "flowbite";

@Injectable({
  providedIn: 'root'
})
export class ModalHelperService {

  constructor() {
  }

  public initializeModal(modalId: string): Modal {
    const $modalElement: HTMLElement = document.querySelector(modalId)!;

    const modalOptions: ModalOptions = {
      placement: 'top-center',
      backdrop: 'dynamic',
      backdropClasses: 'bg-gray-900 bg-opacity-50 dark:bg-opacity-80 fixed inset-0 z-40',
      closable: true,
      onHide: () => {
        console.log('modal is hidden');
      },
      onShow: () => {
        console.log('modal is shown');
      },
      onToggle: () => {
        console.log('modal has been toggled');
      }
    }
    return new Modal($modalElement, modalOptions);
  }

}
