import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  apiURL : string = 'http://localhost:4379';
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
    return this.http.post(this.apiURL + "/api/auth/register" ,user);
  }
  
}
