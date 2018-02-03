import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { Registration } from '../shared/registration';

@Component({
  selector: 'am-registrations',
  templateUrl: './registrations.component.html',
  styles: []
})
export class RegistrationsComponent implements OnInit {

  displayedColumns = ['login', 'salutation', 'title', 'firstName', 'lastName', 'email'];
  dataSource = new MatTableDataSource<Registration>(ELEMENT_DATA);

  constructor() { }

  ngOnInit() {
  }

}

const ELEMENT_DATA: Registration[] = [
  { id: 'ABC', login: 'jezhik-laas', salutation: 'Herr', firstName: 'Uwe', lastName: 'Laas', email: 'uwe.laas@domla.de', title: ''},
];
