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
                <td>{{price.discount}}</td>
                <td>{{price.profit}}</td>
            </tr>
        </tbody>
    </table>
</template>

<script>
    import Vue from 'vue';

    export default {
        data: () => {
            return {
                prices: null
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
            }
        }
    }

</script>