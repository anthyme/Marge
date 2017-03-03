<template>
    <table id="pricingTable" class="">
        <thead>
            <tr>
                <th>Amount</th>
                <th>Discount</th>
                <th>Profit</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="price in prices" track-by="id">
                <td>{{price.amount}}</td>
                <input type="text" v-model="price.discount"></input>
                <td>{{price.profit}}</td>
                <input type="button" v-on:click.prevent="changeDiscount(price)" class="btn btn-primary" value="Ok"></input>
            </tr>
        </tbody>
    </table>
</template>

<script>
    import Vue from 'vue';

    export default {
        data: () => {
            return {
                prices: []
            }
        },
        created: function () {
            this.fetchPrices()
        },

        methods: {
            fetchPrices: function () {
                var self = this
                return Vue.http.get('http://localhost:49842/api/price')
                    .then((response) => {
                        self.prices = response.body
                    })
            },
            changeDiscount(price) {
                var self = this
                Vue.http.put('http://localhost:49842/api/price/' + price.id, { discount: price.discount })
                    .then((response) => self.fetchPrices())
            },
        }
    }

</script>