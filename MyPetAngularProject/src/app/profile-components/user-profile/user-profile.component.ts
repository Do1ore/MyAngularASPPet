import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {ToastrService} from "ngx-toastr";
import {HttpClient, HttpEventType} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {ChatImageService} from "../../services/image/chat-image.service";
import {DomSanitizer, SafeUrl} from "@angular/platform-browser";
import {SignalRMessageService} from "../../services/signal-r-message.service";
import {UserImageService} from "../../services/image/user-image.service";


@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  public baseApiPath: string = environment.baseApiUrl;
  public message: string = '';
  public progress: number = 0;
  @Output() public onUploadFinished = new EventEmitter();

  constructor(private http: HttpClient,
              public userImageService: UserImageService,
              private sanitizer: DomSanitizer,
              private signalRService: SignalRMessageService) {
  }

  public imageUrl!: SafeUrl;

  ngOnInit(): void {
    this.getProfileImage();
  }

  public getProfileImage() {
    this.userImageService.getCurrentUserProfileImage().subscribe(
      (imageBlob: Blob) => {
        // Создаем безопасный URL для изображения
        const objectUrl: string = URL.createObjectURL(imageBlob);
        this.imageUrl = this.sanitizer.bypassSecurityTrustUrl(objectUrl);
      },
      (error: any) => {
        console.error('Ошибка при загрузке изображения:', error);
      }
    );
  }

  // @ts-ignore
  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('image', fileToUpload);
    formData.append('userId', this.signalRService.getUserIdFromToken())

    this.userImageService.uploadUserProfileImage(formData).subscribe(() => {
      console.log('Uploaded')
    });
  }


}

