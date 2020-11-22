import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RoleModel } from '../models/role.model';
import { UserModel } from '../models/user.model';
import { UserService } from '../shared/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  
  role : RoleModel = {
      id : 0,
      name : ''
  }

  constructor(private router: Router, private userService:UserService) { }

  ngOnInit(): void {
    
    this.loadCurrentUserRole();
  }

  onLogout() {
    localStorage.removeItem('token');
    this.router.navigate(['/user/login']);
  }

  loadCurrentUserRole() {
    this.role.name = this.userService.getCurrentUserRole();
  }
}
