import { AppUser } from "./appUser";

export interface UserProfileImage {
  imageId: string;
  appUser: AppUser | null;
  appUserId: string;
  fileName: string | null;
  imagePath: string | null;
  formImage: FormData | null;
}
