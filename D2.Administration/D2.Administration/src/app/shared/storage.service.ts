import { Injectable } from '@angular/core';

@Injectable()
export class StorageService {

  constructor() { }

  set(key: string, value: string) {
    window.localStorage.setItem(key, value);
  }

  get(key: string): string {
    return window.localStorage.getItem(key);
  }
}
