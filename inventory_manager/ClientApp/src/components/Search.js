import React, { Component } from 'react';

export class Search extends Component {
    static displayName = Search.name;

    constructor(props) {
        super(props);
        this.state = {
            searchName: '', results: undefined, loading: false };

        this.handleInput = this.handleInput.bind(this);
        this.handleSearch = this.handleSearch.bind(this);
    }

    componentDidMount() {
        this.fetchInventoryData();
    }

    static renderInventory(item) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Name</th>
                        <th>Price</th>
                    </tr>
                </thead>
                <tbody> 
                    {item ?
                        <tr>
                            <td>{item.id}</td>
                            <td>{item.name}</td>
                            <td>${item.cost}</td>
                        </tr>
                        : ''}
                </tbody>
            </table>
        );
    }

    handleInput(event) {
        this.setState({ searchName: event.target.value })
    }
    handleSearch(event) {
        this.fetchInventoryData();
    }

    render() {
        const { results, loading } = this.state;
 
        let contents = '';

        if (loading) {
            contents = <p><em>Loading...</em></p>
        }
        else if (results) {
            if (results.status && results.status == 404) {
                contents = <p><em>Could not find any item by that name, please check the spelling and try again</em></p>
            }
            else {
                contents = Search.renderInventory(results)
            }
        }

        return (
            <div>
                <h1 id="tabelLabel" >Widget Stocks</h1>
                <p>Is there a specific item you had in mind?</p>

                <label htmlFor="nameInput">Item Name:&nbsp;&nbsp;</label>
                <input type="text" id="nameInput" name="nameInput" value={this.state.searchName} onChange={this.handleInput} hint="blah" />&nbsp;
                <button type="button" onClick={this.handleSearch}>Search</button>

                {contents}
            </div>
        );
    }

    async fetchInventoryData() {
        const { searchName } = this.state;

        if (searchName && searchName.length) {
            this.setState({loading: true})
            const response = await fetch(`api/inventory/max/${searchName}`);
            const data = await response.json();
            this.setState({ results: data, loading: false });
        }
    }
}
