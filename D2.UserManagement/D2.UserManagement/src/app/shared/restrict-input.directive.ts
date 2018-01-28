import { Directive, ElementRef, Input } from '@angular/core';
import * as Inputmask from 'inputmask';

@Directive({
  selector: '[um-restrict-input]',
})
export class RestrictInputDirective {

  private regexMap = {
    integer: '^[0-9]*$',
    float: '^[+-]?([0-9]*[.])?[0-9]+$',
    words: '([A-z]*\\s)*',
    point25: '^\-?[0-9]*(?:\\.25|\\.50|\\.75|)$',
    username: '^[a-z_]+[0-9a-z_\\\\-]*$'
  };

  constructor(private el: ElementRef) {}

  @Input('um-restrict-input')
  public set defineInputType(type: string) {
    Inputmask({regex: this.regexMap[type], placeholder: ''})
      .mask(this.el.nativeElement);
  }
}
