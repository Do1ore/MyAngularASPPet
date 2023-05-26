import {AppUser} from "./appUser";

export class UserProfileImage {
  public imageId: string = "";
  public appUser: AppUser | null = null;
  public appUserId: string = "";
  public fileName: string | null = "";
  public imagePath: string | null = "";
  public formImage: FormData | null = null;
}
