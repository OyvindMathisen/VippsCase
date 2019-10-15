import { Component, OnInit } from '@angular/core';
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: 'app-login-module',
  templateUrl: './login-module.component.html',
  styleUrls: ['./login-module.component.scss']
})

@Injectable()
export class LoginModuleComponent implements OnInit {

  constructor(private http: HttpClient) { }

  ngOnInit() {
  }
  
}
