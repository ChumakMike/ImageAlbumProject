import { Component, OnInit } from '@angular/core';
import { UserModel } from 'src/app/models/user.model';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-home-profile',
  templateUrl: './home-profile.component.html',
  styleUrls: ['./home-profile.component.css']
})
export class HomeProfileComponent implements OnInit {
  
  userModel : UserModel;
  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.loadCurrentUserData();
  }

  loadCurrentUserData() {
    this.userService.getCurrentUserDataById().subscribe(
      (res:any) => {
        this.userModel = res;
      }, 
      err => {
        console.log(err);
      }
    );
  }
}
