import { UserService } from './../../_services/user.service';
import { AuthService } from './../../_services/auth.service';
import { environment } from './../../../environments/environment';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { Photo } from 'src/app/_models/Photo';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-photo-edit',
  templateUrl: './photo-edit.component.html',
  styleUrls: ['./photo-edit.component.css']
})
export class PhotoEditComponent implements OnInit {

  @Input() photos: Photo[];
  @Output() getMemberPhotoChange = new EventEmitter<string>();
  uploader: FileUploader;
  hasBaseDropZoneOver: boolean;
  response: string;
  baseUrl = environment.apiUrl;
  currentMain: Photo;

  constructor(private authService: AuthService,
              private userService: UserService,
              private alertify: AlertifyService) {
    this.initializeUploader();
  }

  ngOnInit() {
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'user/' + this.authService.decodedToken.nameid + '/photos',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024

    });

    this.hasBaseDropZoneOver = false;
    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    // So as to show the photo to user once it is successfully uploaded
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res = JSON.parse(response);

        console.log(res);

        // for mapping
        const photo = {
          id: res.id,
          description: res.description,
          url: res.url,
          dateAdded: res.dateAdded,
          isMainPhoto: res.isMainPhoto
        };

        this.photos.push(photo);
        if (photo.isMainPhoto) {
          this.authService.changeMainPhotoURl(photo.url);
          this.authService.currentUser.photoUrl = photo.url;
          localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
        }
      }
    };
  }

  setMainPhoto(photo: Photo) {
    this.userService.setMainPhoto(this.authService.decodedToken.nameid, photo.id)
    .subscribe(() => {
      this.currentMain = this.photos.filter(p => p.isMainPhoto)[0];
      this.currentMain.isMainPhoto = false;
      photo.isMainPhoto = true;
      // this.getMemberPhotoChange.emit(photo.url);
      this.authService.changeMainPhotoURl(photo.url);
      this.authService.currentUser.photoUrl = photo.url;
      localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
    },
    err => {
      this.alertify.error(err);
    });
  }

  deletePhoto(id: number) {
    console.log(id);
    this.alertify.confirm('Are you sure you want to delete?', () => {
      this.userService.deletePhoto(this.authService.decodedToken.nameid, id)
      .subscribe(() => {
        this.photos.splice(this.photos.findIndex(p => p.id === id), 1);
        this.alertify.success('Photo has been deleted');
      }, err => {
        this.alertify.error('Failed to delete photo');
      });
    });
  }

}
