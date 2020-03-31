import React, { Component } from 'react';

export class Inventory extends Component {
    static displayName = Inventory.name;

    constructor(props) {
        super(props);
        this.state = { inventory: [], loading: true, maxOnly: true };
    }

    componentDidMount() {
        this.fetchInventoryData();
    }

    static renderInventory(items) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Name</th>
                        <th>Cost</th>
                    </tr>
                </thead>
                <tbody>
                    {items.map(item =>
                        <tr key={item.id}>
                            <td>{item.id}</td>
                            <td>{item.name}</td>
                            <td>{item.cost}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Inventory.renderInventory(this.state.inventory);

        return (
            <div>
                <h1 id="tabelLabel" >Widget Stocks</h1>
                <p>Here are all of the widgets we have for sale, and their greatly affordable prices</p>
                {contents}
            </div>
        );
    }

    async fetchInventoryData() {
        const response = await fetch(`api/inventory${this.state.maxOnly ? '/max' : ''} `);
        const data = await response.json();
        console.log(data);
        this.setState({ inventory: data, loading: false });
    }
}
