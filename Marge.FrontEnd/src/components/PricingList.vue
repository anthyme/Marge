<template>
    <table id="pricingTable" class="table table-hover product-table">Pricing
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
    export default {
        data() {
            return {
                prices: [
                    {
                        id: 'cc919e21-ae5b-5e1f-d023-c40ee669520c',
                        amount: 69.,
                        discount: 0.,
                        profit: 42.,
                    },
                    {
                        id: 'bcd755a6-9a19-94e1-0a5d-426c0303454f',
                        amount: 666,
                        discount: 69,
                        profit: 100.42,
                    }
                ]
            }
        }
    }

</script>