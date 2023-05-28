import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {ToastrService} from "ngx-toastr";
import {HttpClient, HttpEventType} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {UserProfileService} from "../../services/user-profile.service";
import {DomSanitizer, SafeUrl} from "@angular/platform-browser";


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

  constructor(private http: HttpClient, public userProfile: UserProfileService, private sanitizer: DomSanitizer) {
  }

  public imageUrl!: SafeUrl;

  ngOnInit(): void {
    this.getProfileImage();
  }

  public getProfileImage() {
    this.userProfile.getImage().subscribe(
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
    formData.append('file', fileToUpload, fileToUpload.name);

    this.http.post(this.baseApiPath + 'api' + '/Account' + '/upload', formData, {
      reportProgress: true,
      observe: "events",
    }).subscribe(event => {
      if (event.type === HttpEventType.UploadProgress) {
        this.progress = Math.round(100 * event.loaded / event.total!);
      } else if (event.type === HttpEventType.Response) {
        this.message = 'Upload success';
        this.onUploadFinished.emit(event.body);
      }
    })
  }

}

