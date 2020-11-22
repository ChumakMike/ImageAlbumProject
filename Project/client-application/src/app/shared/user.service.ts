import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { RoleModel } from '../models/role.model';
import { UserModel } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUri : string = 'http://localhost:4379';

  constructor(private fb: FormBuilder, private http: HttpClient ) { }

  formModel = this.fb.group({
    UserName : ['', Validators.required],
    Email : ['', [Validators.email, Validators.required]],
    Passwords : this.fb.group({
      Password : ['', [Validators.required, Validators.minLength(5)]],
      ConfirmPassword : ['', Validators.required]  
    },
    {validator : this.comparePasswordsFielsValues })
  });

  comparePasswordsFielsValues(fg:FormGroup) {
    let passwordConfirmation = fg.get('ConfirmPassword');
    if(passwordConfirmation.errors == null || 'passwordMismatch' in passwordConfirmation.errors) {
      if(passwordConfirmation.value != fg.get('Password').value) {
        passwordConfirmation.setErrors({'passwordMismatch' : true})
      } else {
        passwordConfirmation.setErrors(null);
      }
    }
  }

  register() {
    let user = {
      UserName : this.formModel.value.UserName,
      Email : this.formModel.value.Email,
      Password : this.formModel.value.UserName
    }
    return this.http.post(this.baseUri + "/api/auth/register" ,user);
  }

  login(data) {
      return this.http.post(this.baseUri + "/api/auth/login" , data);
  }

  getCurrentUserDataById() {
    return this.http.get(this.baseUri + '/api/users/profile');
  }

  getCurrentUserRole() {
    let payload = JSON.parse(window.atob(localStorage.getItem('token').split('.')[1]));
    return payload.role.toString();
  }

  isUserInRole(roles) {
    let isInRole = false;
    let payload = JSON.parse(window.atob(localStorage.getItem('token').split('.')[1]));
    let role = payload.role;
    roles.forEach(element => {
      if(role == element)
        isInRole = true;
    });
    return isInRole;
  }

  getAllUsers() {
    return this.http.get(this.baseUri + '/api/users');
  }
}
