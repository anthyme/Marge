<template>
    <table id="pricingTable" class="">
        <thead>
            <tr>
                <th>Amount</th>
                <th>Discount</th>
                <th>Profit</th>
                <th></th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="price in prices" track-by="id">
                <td>{{price.amount}}</td>
                <input type="text" v-model="price.discount"></input>
                <td>{{price.profit}}</td>
                <td><input type="button" v-on:click.prevent="changeDiscount(price)" class="btn btn-primary" value="Update discount"></input></td>
                <td><a href="#" v-on:click.prevent.stop="deletePrice(price)">remove</a></td>
            </tr>
        </tbody>
    </table>
</template>

<script>
    import Vue from 'vue';

    export default {
        data: () => ({
            prices: null
        }),
        mounted: function () {
            this.fetchPrices()
        },

        methods: {
            fetchPrices: function () { 
                this.$http.get('http://localhost:49842/api/price')
                    .then((response) => this.prices = response.body) 
            },
            changeDiscount(price) {
                this.$http.put('http://localhost:49842/api/price/' + price.id, { discount: price.discount })
                    .then()
            },
            deletePrice(price) {
                this.$http.delete('http://localhost:49842/api/price/' + price.id)
                    .then(this.fetchPrices)
            }
        }
    }
</script>