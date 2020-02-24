import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from './../_services/alertify.service';
import { UserService } from './../_services/user.service';
import { AuthService } from './../_services/auth.service';
import { Component, OnInit } from '@angular/core';
import { User } from '../_models/User';
import { Pagination } from '../_models/pagination';
import { PaginatedResult } from '../_models/PaginatedResult';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  users: User[];
  pagination: Pagination;
  followParam: string;


  constructor(private authService: AuthService,
              private userService: UserService,
              private alertify: AlertifyService,
              private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data =>{
      this.users = data['users'].result;
      this.pagination = data['users'].pagination;
    });

    //this.followParam = 'Followers';
    this.followParam = 'Followings';
    console.log(this.pagination);
  }

  loadUsers() {
    console.log(this.followParam);
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage,  null,  this.followParam)
    .subscribe((result: PaginatedResult<User[]>) => {
      this.users = result.result;
      this.pagination = result.pagination;
      console.log(this.pagination);
    }, err => {
      this.alertify.error(err);
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

}
