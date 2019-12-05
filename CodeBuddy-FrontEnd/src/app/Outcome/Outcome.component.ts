import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-Outcome',
  templateUrl: './Outcome.component.html',
  styleUrls: ['./Outcome.component.css']
})
export class OutcomeComponent implements OnInit {
  result: any;
  constructor(private httpClient: HttpClient) { }

  ngOnInit() {
    this.getValues();
  }

  getValues() {
    this.httpClient.get('https://localhost:44351/api/outcome').subscribe(
      response => {
        this.result = response;
      },
      error => {
        console.log(error);
      });
  }
}
