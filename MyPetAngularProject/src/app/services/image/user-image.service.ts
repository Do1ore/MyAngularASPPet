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
  public currentUserSafeImageUrl: SafeUrl = '';

  constructor(private http: HttpClient, private sanitizer: DomSanitizer) {
  }

  public getCurrentUserProfileImage(): Observable<Blob> {
    const headers = new HttpHeaders().set('Accept', 'image/jpeg');

    return this.http.get(this.baseApiUrl + "/get-current-user-image", {
      headers: headers,
      responseType: 'blob'
    });
  }

  public getUserImageById(userId: string): Observable<Blob> {
    const headers = new HttpHeaders().set('Accept', 'image/jpeg');

    return this.http.get(this.baseApiUrl + "/get-user-image/" + userId, {
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
        userProfileImage.SafeImagePath = this.sanitizer.bypassSecurityTrustUrl(safeUrl);

        this.userProfileImages.push(userProfileImage);
      });
    })
  }

  public updateUserProfileImageUrl(imagePath: string, userId: string): void {
    const userIndex = this.userProfileImages.findIndex(user => user.Id === userId);

    if (userIndex !== -1) {
      this.userProfileImages[userIndex].SafeImagePath = imagePath as SafeUrl;
    } else {
      const newUserProfileImage = new UserProfileImage();
      newUserProfileImage.Id = userId;
      newUserProfileImage.SafeImagePath = imagePath as SafeUrl;
      this.userProfileImages.push(newUserProfileImage);
    }
  }

  public findUserProfileById(userId: string) {
    let safeImagePath = this.userProfileImages.find(a => a.Id == userId)?.SafeImagePath;

    if (!safeImagePath) {
      return null;
    }
    return safeImagePath;
  }

}
