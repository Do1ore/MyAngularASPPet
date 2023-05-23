import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {ToastrService} from "ngx-toastr";
import {HttpClient, HttpEventType} from "@angular/common/http";
import {environment} from "../../../environments/environment";




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

  constructor(private http: HttpClient) {
  }

  ngOnInit(): void {

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
      if(event.type === HttpEventType.UploadProgress){
        this.progress = Math.round(100 * event.loaded / event.total!);
      }
      else if(event.type === HttpEventType.Response){
        this.message = 'Upload success';
        this.onUploadFinished.emit(event.body);
      }
    })
  }

}

