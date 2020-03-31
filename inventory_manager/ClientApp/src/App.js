import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';


import './custom.css'
import { Inventory } from './components/Inventory';
import { Search } from './components/Search';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
            <Route exact path='/' component={Inventory} />
            <Route path='/search' component={Search} />
      </Layout>
    );
  }
}
