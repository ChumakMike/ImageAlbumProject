import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  baseUri : string = 'http://localhost:4379';

  constructor(private http: HttpClient) { }

  getAllRoles() {
    return this.http.get(this.baseUri + '/api/roles');
  }
}
