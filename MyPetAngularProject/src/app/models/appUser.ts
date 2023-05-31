import {ChatMessage} from "./chatMessage";


import {UserProfileImage} from "./userProfileImage";

export class AppUser {
  public id: string = "";
  public username: string = "";
  public surname: string = "";
  public email: string = "";
  public passwordHash: string = "";
  public passwordSalt: string = "";
  public refreshToken: string = "";
  public tokenCreated: string = "";
  public tokenExpires: string = "";
  public accountCreated: string = "";
  public accountLastTimeEdited: string = "";
  public lastTimeOnline: string = "";
  public imageURL: string = '';
  public userProfileImages: UserProfileImage[] = [];
  public sentMessages: ChatMessage[] = [];
  public receivedMessages: ChatMessage[] = [];
}
