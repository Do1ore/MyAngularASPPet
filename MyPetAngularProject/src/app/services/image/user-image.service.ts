import {Injectable} from '@angular/core';
import {Observable} from "rxjs";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {UserProfileImage} from "../../models/local/userProfileImage";
import {AppUser} from "../../models/appUser";
import {DomSanitizer, SafeUrl} from "@angular/platform-browser";

@Injectable({
  providedIn: 'root'
})
export class UserImageService {

  baseApiUrl: string = environment.baseApiUrl + "api/" + 'Image';
  public userProfileImages: UserProfileImage[] = [];

  constructor(private http: HttpClient, private sanitizer: DomSanitizer) {
  }

  public getCurrentUserProfileImage(): Observable<Blob> {
    const headers = new HttpHeaders().set('Accept', 'image/jpeg');

    return this.http.get(this.baseApiUrl + "/get-current-user-image", {
      headers: headers,
      responseType: 'blob'
    });
  }

  public uploadUserProfileImage(formData: FormData) {
    return this.http.post(this.baseApiUrl + '/upload-user-image', formData);
  }

  public async getUserProfileImages(users: AppUser[]) {

    const headers = new HttpHeaders().set('Accept', 'image/jpeg');

    users.forEach((u) => {
      this.http.get(this.baseApiUrl + '/get-user-image/' + u.id, {
        headers: headers,
        responseType: 'blob'
      }).subscribe((imageBlob) => {
        let safeUrl = URL.createObjectURL(imageBlob);
        let userProfileImage = new UserProfileImage();
        userProfileImage.Id = u.id;
        userProfileImage.SafaImagePath = this.sanitizer.bypassSecurityTrustUrl(safeUrl);

        this.userProfileImages.push(userProfileImage);
      });
    })
  }

  public getUserProfileById(userId: string) {
    let userProfileImageSageUrl = this.userProfileImages.find(a => a.Id == userId)?.SafaImagePath;

    if (!userProfileImageSageUrl) {
      return null;
    }
    return userProfileImageSageUrl;
  }

}
