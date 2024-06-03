using EmployeeService.DataAcess.Entities;
using EmployeeService.DataAcess.Enums;
using EmployeeService.Service.Services.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeService.Services.Tests
{
    public class ExpressionBuilderServiceTests
    {
        private IExpressionBuilderService _sut;

        public ExpressionBuilderServiceTests()
        {
           _sut = new ExpressionBuilderService();
        }

        [Fact]
        public void CreteFilterByExpression_WithAllParamsNull_ReturnNull()
        {
            // Arrange

            // Act
            var result = _sut.CreteFilterByExpression(null, null, null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CreteFilterByExpression_WithValueNull_ReturnNull()
        {
            // Arrange

            // Act
            var result = _sut.CreteFilterByExpression(Filter.Equals, Field.EmployeeId, null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CreteFilterByExpression_WithFieldNull_ReturnNull()
        {
            // Arrange
            var value = 5;

            // Act
            var result = _sut.CreteFilterByExpression(Filter.Equals, null, value);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CreteFilterByExpression_WithFilterNull_ReturnNull()
        {
            // Arrange
            var value = 5;

            // Act
            var result = _sut.CreteFilterByExpression(null, Field.EmployeeId, value);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CreteFilterByExpression_WithCorrentInput_ReturnExpression()
        {
            // Arrange
            object value = 5;

            // Act
            var result = _sut.CreteFilterByExpression(Filter.Equals, Field.EmployeeId, value);

            // Assert
            Assert.NotNull(result);
        }
    }
}
