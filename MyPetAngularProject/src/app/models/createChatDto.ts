
export class CreateChatDto {
  public chatName: string = ""
  public userIds: string[] = [];
  public creatorId: string = "";
  public chatImage: File | null = null;
}
