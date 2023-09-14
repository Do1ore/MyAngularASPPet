import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {DomSanitizer, SafeUrl} from "@angular/platform-browser";
import {UserImageService} from "../../services/image/user-image.service";
import {LocalStorageHelperService} from "../../services/local-storage-helper.service";
import {ActivatedRoute} from "@angular/router";
import {AppUser} from "../../models/appUser";
import {UserService} from "../../services/user.service";


@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {

  //profile owner id
  userId: string | null = '';
  isCreator: boolean = false;

  @Output() public onUploadFinished = new EventEmitter();

  public message: string = '';
  public progress: number = 0;
  public userData: AppUser = new AppUser();


  constructor(public userImageService: UserImageService,
              private sanitizer: DomSanitizer,
              private storageService: LocalStorageHelperService,
              private route: ActivatedRoute,
              private userService: UserService) {
  }

  public imageUrl!: SafeUrl;

  ngOnInit(): void {
    this.userId = this.route.snapshot.paramMap.get('userid');

    this.isCreator = this.isUserProfileOwner();
    if (this.isCreator)
      this.getCurrentUserProfileImage();
    else {
      console.log('userId: ', this.userId)

      this.getUserProfileImage(this.userId!);
    }
    this.getUserData();
  }

  isUserProfileOwner() {
    return this.isCreator = this.userId === this.storageService.getUserIdFromToken();
  }

  public getCurrentUserProfileImage() {
    this.userImageService.getCurrentUserProfileImage().subscribe(
      (imageBlob: Blob) => {

        const objectUrl: string = URL.createObjectURL(imageBlob);
        this.imageUrl = this.sanitizer.bypassSecurityTrustUrl(objectUrl);
      },
      (error: any) => {
        console.error('Ошибка при загрузке изображения:', error);
      }
    );
  }

  public getUserProfileImage(userId: string) {
    this.userImageService.getUserImageById(userId).subscribe(
      (imageBlob: Blob) => {

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
    formData.append('userId', this.storageService.getUserIdFromToken())

    this.userImageService.uploadUserProfileImage(formData).subscribe(() => {
      console.log('Uploaded')
    });
  }

  public getUserData() {
    this.userService.getUserById(this.userId!).subscribe((user) => {
      this.userData = user;
      console.log('User data: ', this.userData)
    });
  }

}

