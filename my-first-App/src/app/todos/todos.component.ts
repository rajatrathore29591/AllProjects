

  import { Component, OnInit  } from '@angular/core';

  @Component({
    selector: 'app-todos',
    templateUrl: './todos.component.html',
    styleUrls: ['./todos.component.css']
  })

  export class TodosComponent implements OnInit  {

    newTodo: string;
    todos: any;
    todoObj: any;

    constructor() {
      this.newTodo = '';
      this.todos = [];
    }

     ngOnInit() {
  }

    addTodo(event) {
      this.todoObj = {
        newTodo: this.newTodo,
        completed: false
      }
      this.todos.push(this.todoObj);
      this.newTodo = '';
      event.preventDefault();
    }

    deleteTodo(index) {
      this.todos.splice(index, 1);
    }

    deleteSelectedTodos() {
      //need ES5 to reverse loop in order to splice by index
      for(var i=(this.todos.length -1); i > -1; i--) {
        if(this.todos[i].completed) {
          console.log(this.todos[i].completed)
          this.todos.splice(i, 1);
        }
      }
    }

  }